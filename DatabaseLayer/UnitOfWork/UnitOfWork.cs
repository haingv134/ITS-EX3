using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseLayer.Context;
using DatabaseLayer.Repository;
using Microsoft.Extensions.Logging;
using DatabaseLayer.ExceptionHandling;

namespace DatabaseLayer.UnitOfWork
{
    public class UnitOfWork : IDisposable
    {
        private DatabaseContext dbContext;
        private ClassRepository classRepository;
        private StudentRepository studentRepository;
        private SubjectRepository subjectRepository;
        private ClassStudentRepository classStudentRepository;
        private ClassSubjectRepository classSubjectRepository;        

        // inject dbcontext from DI
        public UnitOfWork(DatabaseContext dbcontext)
        {
            this.dbContext = dbcontext;
            this.classRepository = new ClassRepository(dbcontext);
            this.studentRepository = new StudentRepository(dbcontext);
            this.subjectRepository = new SubjectRepository(dbcontext);
            this.classStudentRepository = new ClassStudentRepository(dbcontext);
            this.classSubjectRepository = new ClassSubjectRepository(dbcontext);
        }

        public ClassRepository ClassRepository { get => classRepository; set => classRepository = value; }
        public StudentRepository StudentRepository { get => studentRepository; set => studentRepository = value; }
        public SubjectRepository SubjectRepository { get => subjectRepository; set => subjectRepository = value; }
        public ClassStudentRepository ClassStudentRepository { get => classStudentRepository; set => classStudentRepository = value; }
        public ClassSubjectRepository ClassSubjectRepository { get => classSubjectRepository; set => classSubjectRepository = value; }

        // return number of row effected
        public async Task<int> SaveChange()
        {
            int _rowEffected = 0;
            try
            {
                _rowEffected = await dbContext.SaveChangesAsync();
            }
            catch(Exception e)
            {
                dbContext.Dispose();
                throw new CustomeException(e.Message);
            }
            return _rowEffected;
        }
        // release memory if saveChange() has error
        public void Dispose()
        {
            dbContext.Dispose();
        }
    }
}
